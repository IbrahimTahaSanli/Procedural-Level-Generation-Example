using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Procedural
{
    private GameObject _gameobject { get; set; }

    public Procedural(GameObject go)
    {
        _gameobject = go;
    }
    
    public void CreatePlatform(int seed)
    {
        System.Random random = new System.Random(seed);
        
        if(!AssetBundleManager.TryGetAssetBundle("procedural_materials")) return;

        List<GameObject> gameobjects = new List<GameObject>(AssetBundleManager.GetAll<GameObject>("procedural_materials"));

        float point = PlayerStatus.Character.GetComponent<PointManager>().point;
        int posibilty;

        if (point < 10000)
            posibilty = (int)((_gameobject.transform.Find("Left").Find("Top").transform.position.y - _gameobject.transform.Find("Left").Find("Bottom").transform.position.y)/2) * 2;
        else if (point < 100000)
            posibilty = (int)((_gameobject.transform.Find("Left").Find("Top").transform.position.y - _gameobject.transform.Find("Left").Find("Bottom").transform.position.y)/2) * 2 / ((int)point / 10000 );
        else
            posibilty = (int)((_gameobject.transform.Find("Left").Find("Top").transform.position.y - _gameobject.transform.Find("Left").Find("Bottom").transform.position.y)/2) * 2 / 10;

        var length = (int)(_gameobject.transform.Find("Left").Find("Top").transform.position.y - _gameobject.transform.Find("Left").Find("Bottom").transform.position.y);
        var x = _gameobject.transform.Find("Left").Find("Colli").GetComponent<BoxCollider2D>().bounds.extents.x +  _gameobject.transform.Find("Left").Find("Colli").GetComponent<BoxCollider2D>().offset.x;

        for (int i = -length/4; i < length/4 ; i++)
        {
            if (random.Next(1, posibilty) == 1)
            {
                var tmp = GameObject.Instantiate(gameobjects.Find(value => value.name.StartsWith("Block-Left")), _gameobject.transform.Find("Left"));
                tmp.transform.localPosition = new Vector3(x, i*2, 0);
            }
        }

        length = (int)(_gameobject.transform.Find("Right").Find("Top").transform.position.y - _gameobject.transform.Find("Right").Find("Bottom").transform.position.y);
        x = _gameobject.transform.Find("Right").Find("Colli").GetComponent<BoxCollider2D>().bounds.extents.x + _gameobject.transform.Find("Right").Find("Colli").GetComponent<BoxCollider2D>().offset.x;

        for (int i = -length / 4; i < length / 4; i++) { 
            if (random.Next(1, posibilty) == 1)
            {
                var tmp = GameObject.Instantiate(gameobjects.Find(value => value.name.StartsWith("Block-Right")), _gameobject.transform.Find("Right"));
                tmp.transform.localPosition = new Vector3(x,i*2,0);
            }
        }

        gameobjects.Remove(gameobjects.Find(val => val.name.StartsWith("Block-Left")));
        gameobjects.Remove(gameobjects.Find(val => val.name.StartsWith("Block-Right")));

        Dictionary<GameObject, int> middles = new Dictionary<GameObject, int>();

        for(int i = 0; i < gameobjects.Count; i++)
        {
            var tmp = 0;
            foreach (GameObject go in middles.Keys)
                tmp += middles[go]; 

            var str = gameobjects[i].name.Split('-');
            if (point < 10000)
                middles.Add(gameobjects[i], tmp+(int)(float.Parse(str[str.Length - 2]) * 10));
            else if (point < 100000)
                middles.Add(gameobjects[i], tmp+(int)(float.Parse(str[str.Length - 2]) * 10) + (int)((int)(float.Parse(str[str.Length - 2]) * 10) - (int)(float.Parse(str[str.Length - 1]) * 10)) / (100000 - 10000));
            else
                middles.Add(gameobjects[i], tmp+(int)(float.Parse(str[str.Length - 1]) * 10));
        }

        x = _gameobject.transform.Find("Left").Find("Bottom").position.x + 6;

        var xBlock =  (int)(System.Math.Abs((_gameobject.transform.Find("Left").position.x - _gameobject.transform.Find("Right").position.x)) - (2 * - x) - (2 * 2))/2;
        var yBlock = (int)(_gameobject.transform.Find("Left").Find("Top").position.y - _gameobject.transform.Find("Left").Find("Bottom").position.y) / 2;

        posibilty = xBlock * yBlock * 10; 

        for(int i = 0; i < xBlock; i++)
        {
            for(int j = 0; j < yBlock; j++) {
                var number = random.Next(1, posibilty);
                
                foreach(GameObject go in middles.Keys)
                    if(number < middles[go])
                    {
                        var inis = GameObject.Instantiate(go, _gameobject.transform.Find("Left"));
                        inis.transform.position = new Vector2( x + i * 2 , j * 2 );
                        break;
                    }
            }
        }
    }


}
