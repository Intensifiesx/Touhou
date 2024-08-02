using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public List<GameObject> pooledObjects;
    public GameObject obj;
    public int limit;
    void Awake(){
        instance = this;
    }

    void Start(){
        pooledObjects = new List<GameObject>();
        GameObject temp;
        for(int i = 0; i < limit; i++){
            temp = Instantiate(obj, this.transform);
            temp.SetActive(false);
            pooledObjects.Add(temp);
        }
    }

    public GameObject getPooledObject(){
        for(int i = 0; i < limit; i++){
            if(!pooledObjects[i].activeInHierarchy){
                return pooledObjects[i];
            }
        }
        return null;
    }
}
