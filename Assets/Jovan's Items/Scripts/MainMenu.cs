using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button tutorialButton;
    public Button mapButton;
    public Button playButton;
    public Button backButton;
    public Button backButtonLevel;
    public Button backButtonMap;
    public Button quitButton;
    public Transform shutters;
    public Transform cameraPosition;
    public Transform zoomOnBook;
    public Transform zoomOnManual;
    public Transform zoomOnMap;
    public AudioSource shutterSound;
    public GameObject mainMenu;
    public GameObject lobby;
    public GameObject backFromMap;
    public GameObject levelSelection;
    public BoxCollider[] objectsInGame;
    public BoxCollider[] pointers;
    public GameObject[] recipe;
    public GameObject[] instruction;
    public MouseHover mouseHover;
    public Material transparent;
    Coroutine currentCoroutine;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        tutorialButton.onClick.AddListener(Tutorial);
        mapButton.onClick.AddListener(Map);
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
        backButton.onClick.AddListener(Back);
        backButtonLevel.onClick.AddListener(ReturnToLobby);
        backButtonMap.onClick.AddListener(BackMap);
    }
    void Update()
    {
    if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
    {
        HandleInput(Mouse.current.position.ReadValue());
    }
    }
    public void HandleInput(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            // Your button logic
            if (hit.collider.CompareTag("Recipe Book"))
            {
                Debug.Log("LOL");
                StartCoroutine(MoveCameraToRecipe());
                foreach(GameObject book in recipe)
                {
                    foreach(BoxCollider obj in objectsInGame)
                    {
                        if(obj.name == "Instructions Manual" || obj.name == "Map")
                        {
                            obj.enabled = false;
                        }
                    }
                    if(book.name == "Recipe Book Unopened")
                    book.SetActive(false);
                    if(book.name == "Recipe Book Opened")
                    book.SetActive(true);
                }
            }
            if (hit.collider.CompareTag("Instructions"))
            {
                StartCoroutine(MoveCameraToInstructions());
                foreach(GameObject book in instruction)
                {
                    foreach(BoxCollider obj in objectsInGame)
                    {
                        if(obj.name == "Recipe Book" || obj.name == "Map")
                        {
                            obj.enabled = false;
                        }
                    }
                    if(book.name == "Instructions Unopened")
                    book.SetActive(false);
                    if(book.name == "Instructions Opened")
                    book.SetActive(true);
                }
            }
            if(hit.collider.name == "Yishun Pointer" ||
            hit.collider.name == "Marina Bay Pointer" ||
            hit.collider.name == "Changi Airport Pointer" ||
            hit.collider.name == "Scape Pointer" ||
            hit.collider.name == "Jurong East Pointer" ||
            hit.collider.name == "Suntec City Pointer")
            {
                MeshRenderer renderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.green;
                    renderer.material.SetColor("_EmissionColor", Color.black);
                    Datamanager.location = hit.collider.name;
                    foreach(BoxCollider obj in objectsInGame)
                    {
                        if(obj.CompareTag("Pointer") && obj.name != hit.collider.name)
                        {
                            MeshRenderer rendererAgain = obj.gameObject.GetComponent<MeshRenderer>();
                            rendererAgain.material.color = transparent.color;
                        }
                    }
                }
            }
            if(hit.collider.CompareTag("Easy Box"))
            {
                Datamanager.difficulty = "Easy";
                SceneManager.LoadScene(Datamanager.location);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if(hit.collider.CompareTag("Normal Box"))
            {
                Datamanager.difficulty = "Normal";
                SceneManager.LoadScene(Datamanager.location);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if(hit.collider.CompareTag("Hard Box"))
            {
                Datamanager.difficulty = "Hard";
                SceneManager.LoadScene(Datamanager.location);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if(hit.collider.CompareTag("Jovan Box"))
            {
                Datamanager.difficulty = "Jovan";
                SceneManager.LoadScene("Main Game");
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    void StartGame()
    {
        shutterSound.enabled = true;
        if (currentCoroutine != null)
        StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(Open());
    }
    void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Map()
    {
        StartCoroutine(MoveCameraToMap());
                foreach(BoxCollider obj in objectsInGame)
                {
                    if(obj.name == "Recipe Book" || obj.name == "Instructions Manual" || obj.name == "Map")
                    {
                        obj.enabled = false;
                    }
                }
    }
    void PlayGame()
    {
        StartCoroutine(MoveToLevel());
    }
    void QuitGame()
    {
        Application.Quit();
    }
    void Back()
    {
        foreach(BoxCollider obj in objectsInGame)
        {
            obj.enabled = false;
        }
        shutterSound.enabled = true;
        if (currentCoroutine != null)
        StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(Close());
    }
    void ReturnToLobby()
    {
        StartCoroutine(BackFromLevel());
    }
    void BackMap()
    {
        StartCoroutine(BackFromMap());
        StartCoroutine(BackFromBook());
    }
    IEnumerator Open()
    {
        Vector3 start = shutters.localPosition;
        Vector3 target = start + Vector3.up * 4.5f;
        Vector3 cameraStart = cameraPosition.localPosition;
        Vector3 cameraNew = cameraStart + Vector3.up * 0.74f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 0f);
        
        float t = 0;
        
        while (t < 1)
        {
            t += Time.deltaTime;
            shutters.localPosition = Vector3.Lerp(start, target, t);
            cameraPosition.localPosition = Vector3.Lerp(cameraStart, cameraNew, t);
            cameraPosition.rotation = Quaternion.RotateTowards(cameraPosition.rotation,targetRotation, 90 * Time.deltaTime);
            yield return null;
        }
        
        shutters.localPosition = target;
        cameraPosition.localPosition = cameraNew;
        yield return new WaitForSeconds(0.1f);
        shutterSound.enabled = false;
        lobby.SetActive(true);
        foreach(BoxCollider obj in objectsInGame)
        {
            obj.enabled = true;
            if(obj.gameObject.name == "Recipe Book")
            {
                obj.gameObject.tag = "Recipe Book";
            }
            if(obj.gameObject.name == "Instructions Manual")
            {
                obj.gameObject.tag = "Instructions";
            }
            if(obj.gameObject.name == "Map")
            {
                obj.gameObject.tag = "Map";
            }
        }
    }
    IEnumerator Close()
    {
        foreach(BoxCollider obj in objectsInGame)
        {
            if(obj.gameObject.name == "Recipe Book" || obj.gameObject.name == "Instructions Manual" || obj.gameObject.name == "Map")
            {
                obj.gameObject.tag = "Untagged";
            }
        }
        Vector3 end = shutters.localPosition;
        Vector3 endTarget = end + Vector3.down * 4.5f;
        Vector3 cameraEnd = cameraPosition.localPosition;
        Vector3 cameraOld = cameraEnd + Vector3.down * 0.74f;
        Quaternion endTargetRotation = Quaternion.Euler(-20f, 0f, 0f);
        
        float t = 0;
        
        while (t < 1)
        {
            t += Time.deltaTime;
            shutters.localPosition = Vector3.Lerp(end, endTarget, t);
            cameraPosition.localPosition = Vector3.Lerp(cameraEnd, cameraOld, t);
            yield return null;
        }
        
        shutters.localPosition = endTarget;
        cameraPosition.localPosition = cameraOld;
        yield return new WaitForSeconds(0.1f);
        shutterSound.enabled = false;
        while (Quaternion.Angle(cameraPosition.rotation, endTargetRotation) > 0.1f)
        {
            cameraPosition.rotation = Quaternion.RotateTowards(cameraPosition.rotation, endTargetRotation, 90 * Time.deltaTime);
            yield return null;
        }
        mainMenu.SetActive(true);
    }
    IEnumerator MoveToLevel()
    {
        foreach(BoxCollider obj in objectsInGame)
        {
            if(obj.gameObject.name == "Recipe Book" || obj.gameObject.name == "Instructions Manual" || obj.gameObject.name == "Map")
            {
                obj.gameObject.tag = "Untagged";
            }
        }
        Quaternion targetRotation = Quaternion.Euler(0, 90, 0);
        while (Quaternion.Angle(cameraPosition.rotation, targetRotation) > 0.1f)
        {
            cameraPosition.rotation = Quaternion.RotateTowards(cameraPosition.rotation, targetRotation, 90 * Time.deltaTime);
            yield return null;
        }
        levelSelection.SetActive(true);
        foreach(BoxCollider obj in objectsInGame)
        {
            if(obj.gameObject.name == "Easy Box" || 
            obj.gameObject.name == "Normal Box" || 
            obj.gameObject.name == "Hard Box" || 
            obj.gameObject.name == "Jovan Box")
            {
                obj.gameObject.tag = obj.gameObject.name;
            }
        }
    }
    IEnumerator BackFromLevel()
    {
        foreach(BoxCollider obj in objectsInGame)
        {
            if(obj.gameObject.name == "Easy Box" || 
            obj.gameObject.name == "Normal Box" || 
            obj.gameObject.name == "Hard Box" || 
            obj.gameObject.name == "Jovan Box")
            {
                obj.gameObject.tag = "Untagged";
            }
        }
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
        while (Quaternion.Angle(cameraPosition.rotation, targetRotation) > 0.1f)
        {
            cameraPosition.rotation = Quaternion.RotateTowards(cameraPosition.rotation, targetRotation, 90 * Time.deltaTime);
            yield return null;
        }
        foreach(BoxCollider obj in objectsInGame)
        {
            if(obj.gameObject.name == "Recipe Book")
            {
                obj.gameObject.tag = "Recipe Book";
            }
            if(obj.gameObject.name == "Instructions Manual")
            {
                obj.gameObject.tag = "Instructions";
            }
            if(obj.gameObject.name == "Map")
            {
                obj.gameObject.tag = "Map";
            }
        }
        lobby.SetActive(true);
    }
    IEnumerator MoveCameraToRecipe()
    {
        lobby.SetActive(false);
        Vector3 end = zoomOnBook.position;
        while (Vector3.Distance(cameraPosition.position, end) > 0.1f)
        {
            cameraPosition.position = Vector3.MoveTowards(cameraPosition.position, end, 5 * Time.deltaTime);
            yield return null;
        }
        cameraPosition.position = end;
        backFromMap.SetActive(true);
    }
    IEnumerator MoveCameraToInstructions()
    {
        lobby.SetActive(false);
        Vector3 end = zoomOnManual.position;
        while (Vector3.Distance(cameraPosition.position, end) > 0.1f)
        {
            cameraPosition.position = Vector3.MoveTowards(cameraPosition.position, end, 5 * Time.deltaTime);
            yield return null;
        }
        cameraPosition.position = end;
        backFromMap.SetActive(true);
    }
    IEnumerator MoveCameraToMap()
    {
        lobby.SetActive(false);
        Vector3 end = zoomOnMap.position;
        while (Vector3.Distance(cameraPosition.position, end) > 0.1f)
        {
            cameraPosition.position = Vector3.MoveTowards(cameraPosition.position, end, 5 * Time.deltaTime);
            yield return null;
        }
        cameraPosition.position = end;
        foreach(BoxCollider obj in objectsInGame)
        {
            if(obj.gameObject.CompareTag("Pointer"))
            {
                obj.enabled = true;
            }
        }
        backFromMap.SetActive(true);
    }
    IEnumerator BackFromBook()
    {
        foreach(GameObject book in instruction)
        {
            if(book.name == "Instructions Unopened")
            {
                book.SetActive(true);
            }
            else
            {
                book.SetActive(false);
            }
        }
        foreach(GameObject book in recipe)
        {
            if(book.name == "Recipe Book Unopened")
            {
                book.SetActive(true);
            }
            else
            {
                book.SetActive(false);
            }
        }
        Vector3 end = new Vector3(0, 1.74f, -0.7f);
        while (Vector3.Distance(cameraPosition.position, end) > 0.1f)
        {
            cameraPosition.position = Vector3.MoveTowards(cameraPosition.position, end, 5 * Time.deltaTime);
            yield return null;
        }
        cameraPosition.position = end;
        lobby.SetActive(true);
    }
    IEnumerator BackFromMap()
    {
        foreach(BoxCollider obj in objectsInGame)
        {
            if(obj.gameObject.CompareTag("Pointer"))
            {
                obj.enabled = false;
            }
        }
        Vector3 end = new Vector3(0, 1.74f, -0.7f);
        while (Vector3.Distance(cameraPosition.position, end) > 0.1f)
        {
            cameraPosition.position = Vector3.MoveTowards(cameraPosition.position, end, 5 * Time.deltaTime);
            yield return null;
        }
        cameraPosition.position = end;
        lobby.SetActive(true);
        foreach(BoxCollider obj in objectsInGame)
        {
            if(obj.name == "Recipe Book" || obj.name == "Instructions Manual" || obj.name == "Map")
            {
                obj.enabled = true;
            }
        }
    }
}