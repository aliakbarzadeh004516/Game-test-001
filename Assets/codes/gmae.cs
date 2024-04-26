using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Windows;

public class gmae : MonoBehaviour
{

    [SerializeField] Transform boll;
    [SerializeField]  List<Transform> targets;
    [SerializeField] Transform JampTarget;
    [SerializeField] Transform Jamper;
    [SerializeField]  float spead = 0.08f;
    [SerializeField]  List<Transform>  vallsDef;
    [SerializeField] float bollSpead = 0.03f;
    [SerializeField] LayerMask TargetsLayer ;
    [SerializeField] Material vallsmatryal;
    [SerializeField] int chengingColorSpeadcolor = 1;
    List<Transform> valls = new List<Transform>();
    bool is_exist_target = false;
    Transform lastTargat = null;
    Transform target = null;
    Transform lastJamper = null;
    Transform thisJamper = null;
    const int inscrisingSpead = 20;
    const float maxSpead = 15f;
    int tempForSpead ;
    int tempforcoloer = 0;
    bool ISJamp = false,IsDown = false;
    float R=0.95f,G=0.5f,B=0.5f,Ri=-0.01f,Gi=0.007f,Bi=0.013f;
    void Start()
    {
        for(int i =20 ; i>= -4;i-=2){
            Transform valldef = GetRandomItem(vallsDef);
            valls.Add(GameObject.Instantiate(valldef,new Vector3(0,0,i),Quaternion.identity));
        }

        boll.position=new Vector3(0,0.5f,0);
        boll.forward = new Vector3(-1,0,1);


        tempForSpead = 0;

    }

    // Update is called once per frame
    void Update()
    {
        // _______________________make and moveing target____________________________________
        if(!is_exist_target){
            int ran = UnityEngine.Random.Range(0,3);
            if(ran==1){
                target=GameObject.Instantiate(JampTarget,new Vector3(0,0,20),Quaternion.identity);
                int ran2 = UnityEngine.Random.Range(0,4);
                switch(ran2){
                    case 1:
                    thisJamper=GameObject.Instantiate(Jamper,new Vector3(3,0.05f,17.5f),Quaternion.identity);
                    break;
                    case 2:
                    thisJamper=GameObject.Instantiate(Jamper,new Vector3(1,0.05f,17.5f),Quaternion.identity);
                    break;
                    case 3:
                    thisJamper=GameObject.Instantiate(Jamper,new Vector3(-1,00.05f,17.5f),Quaternion.identity);
                    break;
                    default:
                    thisJamper=GameObject.Instantiate(Jamper,new Vector3(-3,0.05f,17.5f),Quaternion.identity);
                    break;
                }
                is_exist_target = true;
            }else{
                Transform temptarget = GetRandomItem(targets);
                target = GameObject.Instantiate(temptarget,new Vector3(0,0,20),Quaternion.identity);
                is_exist_target = true;

            }
        }
        if(target.position.z-boll.position.z<5){

            lastTargat = target;
            is_exist_target = false;
        }
        if(thisJamper!=null){
            thisJamper.position -=new Vector3(0,0,1) * spead *Time.deltaTime;
            if(thisJamper.position.z-boll.position.z<5){

            lastJamper = thisJamper;
            thisJamper = null;
        }
        }
        if(lastJamper!=null){
            lastJamper.position -=new Vector3(0,0,1) * spead *Time.deltaTime;
            if(lastJamper.position.z-boll.position.z<-4){

            Destroy(lastJamper.gameObject);
            lastJamper = null;
        }
        }

        
        target.position -=new Vector3(0,0,1) * spead *Time.deltaTime;
    
        if (lastTargat!= null){
            if(lastTargat.position.z-boll.position.z<-4){

            Destroy(lastTargat.gameObject);
            lastTargat = null;
        }
            lastTargat.position -=new Vector3(0,0,1) * spead *Time.deltaTime;
        }


         // _______________________moveing boll____________________________________
        if(UnityEngine.Input.GetKey(KeyCode.A) && boll.position.x>=-3.9){
            boll.position +=new Vector3(-1,0,0) * bollSpead *Time.deltaTime;
        }
        else if(UnityEngine.Input.GetKey(KeyCode.D) && boll.position.x<=3.9){
            boll.position +=new Vector3(1,0,0) * bollSpead *Time.deltaTime;
        }
        
        if(lastJamper!=null)
            if(boll.position.x - lastJamper.position.x >-0.5 &&boll.position.z - lastJamper.position.z>-0.5 && 
                boll.position.x - lastJamper.position.x <0.5 &&boll.position.z - lastJamper.position.z<0.5){
                ISJamp=true;
            }
        if(ISJamp){
            if(boll.position.y>=3.5){
                IsDown=true;
                ISJamp=false;
            }else{
                boll.position+=new Vector3(0,1,0)*spead*Time.deltaTime;
            }}
        if(IsDown){
            if(boll.position.y<=0.5){
                IsDown=false;
            }else{
                boll.position-=new Vector3(0,1,0)*spead*Time.deltaTime;
            }

        }
         // _______________________moveing vall____________________________________
        foreach(var item in valls){
            item.position-=new Vector3(0,0,1) * spead *Time.deltaTime;
            // if(item.position.z<=-3){
            //     item.position = new Vector3(0,0,20);
            // }    
        }
        if(valls[valls.Count-1].position.z<18){
            Transform valldef = GetRandomItem(vallsDef);
            valls.Add(GameObject.Instantiate(valldef,new Vector3(0,0,20),Quaternion.identity));
        }
        if(valls[0].position.z<=-4){
            Destroy(valls[0].gameObject);
            valls.Remove(valls[0]);
        }
         // _______________________chenging valls color____________________________________
        if(tempforcoloer>=chengingColorSpeadcolor){
        if(R>=0.95 || R<=0.05) Ri*=-1;
        if(G>=0.95 || G<=0.05) Gi*=-1;
        if(B>=0.95 || B<=0.05) Bi*=-1;
        R+=Ri ;
        G+=Gi ;
        B+=Bi ;
        vallsmatryal.color=new Color(R,G,B);
        tempforcoloer=0;
        }else tempforcoloer++;
            
         // _______________________CHENGING SPEAD ____________________________________
        if(tempForSpead>=inscrisingSpead){
            if(spead<maxSpead)
                spead+= 0.01f;
            tempForSpead = 0;
        }   
        else 
            tempForSpead++;
         // _______________________crashing ____________________________________
        Ray ray = new Ray(boll.position,boll.forward);
        if(Physics.Raycast(ray,0.5f+spead*Time.deltaTime ,TargetsLayer))
            EndGame();

    }
    public Transform GetRandomItem(List<Transform>listToRandomize)
    {
        int randomNum = UnityEngine.Random.Range(0, listToRandomize.Count);
        Transform printRandom = listToRandomize[randomNum];
        return printRandom;
    }
    public void EndGame(){
        UnityEditor.EditorApplication.isPlaying = false;
    }
  
}

