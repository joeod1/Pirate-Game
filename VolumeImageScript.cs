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
