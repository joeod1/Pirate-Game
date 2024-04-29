<<<<<<< HEAD:Assets/UI/VolumeImageScript.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VolumeImageScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    public GameObject TextScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void OnPointerEnter(PointerEventData eventData){
        TextScreen.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData){
        TextScreen.SetActive(false);
    }

}
=======
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VolumeImageScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //Shows text when you hover over the image
    [SerializeField]
    public GameObject TextScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void OnPointerEnter(PointerEventData eventData){
        TextScreen.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData){
        TextScreen.SetActive(false);
    }

}
>>>>>>> 1d2f3e3bfc47ccf7de0cbf438d6fcc3b50b5f0b6:VolumeImageScript.cs
