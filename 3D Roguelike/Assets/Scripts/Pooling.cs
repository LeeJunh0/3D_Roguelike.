using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    static public Pooling instance = null;
    public int Pooling_Count;

    public GameObject Player_Bullet;
    public GameObject Textmesh;
    public GameObject[] Enemy_Prefab;

    Queue<GameObject> Player_Bullet_Queue = new Queue<GameObject>();
    Queue<GameObject> Textmesh_Queue = new Queue<GameObject>();
    Queue<GameObject> Enemy_Queue = new Queue<GameObject>();
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
                
    }
    private void Start()
    {
        for(int i = 0; i < Pooling_Count; i++)
        {
            Textmesh_Queue.Enqueue(TextMeshCreat());
            Player_Bullet_Queue.Enqueue(PlayerCreatBullet());
            Enemy_Queue.Enqueue(EnemyCreat());
        }     
    }

    GameObject TextMeshCreat()
    {
        GameObject TempObject = Instantiate(Textmesh);
        TempObject.SetActive(false);
        TempObject.transform.SetParent(this.transform);
        return TempObject;
    }
    GameObject PlayerCreatBullet()
    {
        GameObject TempBullet = Instantiate(Player_Bullet);
        TempBullet.SetActive(false);
        TempBullet.transform.SetParent(this.transform);
        return TempBullet;
    }
    GameObject EnemyCreat()
    {
        GameObject TempEnemy = Instantiate(Enemy_Prefab[Random.Range(0, Enemy_Prefab.Length)]);
        TempEnemy.SetActive(false);
        TempEnemy.transform.SetParent(this.transform);
        return TempEnemy;
    }
    public GameObject GetElement(GameObject Layer)
    {
        GameObject elseObj = null;
        if (Layer.gameObject.tag == "Player")
        {
            if (Player_Bullet_Queue.Count > 0)
            {
                GameObject Bullet = Player_Bullet_Queue.Dequeue();
                Bullet.transform.SetParent(null);
                return Bullet;
            }
            else
            {
                GameObject newBullet = PlayerCreatBullet();
                newBullet.transform.SetParent(null);
                return newBullet;
            }
        }
        if(Layer.gameObject.tag == "Spawner" || Layer.gameObject.tag == "WaveManager")
        {
            if (Enemy_Queue.Count > 0)
            {
                GameObject Bullet = Enemy_Queue.Dequeue();
                Bullet.transform.SetParent(null);
                return Bullet;
            }
            else
            {
                GameObject NewEnemy = EnemyCreat();
                NewEnemy.transform.SetParent(null);
                return NewEnemy;
            }
        }
        return elseObj;
    }
    public void ReturnElement(GameObject Element)
    {
        if (Element.gameObject.tag == "Enermy" || Element.gameObject.tag == "WaveEnemy")
        {
            Element.SetActive(false);
            Element.transform.SetParent(this.transform);
            Enemy_Queue.Enqueue(Element);
        }
        if (Element.gameObject.tag == "Armo")
        {
            Element.SetActive(false);
            Element.transform.SetParent(this.transform);
            Player_Bullet_Queue.Enqueue(Element);
        }
    }
    public GameObject GetTextMesh()
    {
        if (Textmesh_Queue.Count <= 0)
        {
            GameObject NewTextMesh = TextMeshCreat();
            NewTextMesh.transform.SetParent(null);
            NewTextMesh.SetActive(true);
            return NewTextMesh;            
        }
        GameObject Textmesh = Textmesh_Queue.Dequeue();
        Textmesh.transform.SetParent(null);
        Textmesh.SetActive(true);
        return Textmesh;
    }
    public void ReturnTextMesh(GameObject TextObject)
    {
        TextObject.SetActive(false);
        TextObject.transform.SetParent(this.transform);
        Textmesh_Queue.Enqueue(TextObject);
    }

}

