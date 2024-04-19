using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewManagementScreen : MonoBehaviour
{
static Dictionary<string, CrewCard> crew = new Dictionary<string, CrewCard>();
  static CrewCard GetMember(string name) {
     if (crew.ContainsKey("name")){
       return crew[name];}
     else return null;
  }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
