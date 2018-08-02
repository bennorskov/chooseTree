using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using UnityEngine.

public class treeSeedChange : MonoBehaviour {

    private Tree tr;
    private TreePrototype treePrototype;

	// Use this for initialization
	void Start () {
        treePrototype = new TreePrototype();

        Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();

        TreeInstance treeInstance = terrain.terrainData.treeInstances[0];
        print(terrain.terrainData.treePrototypes[0].prefab.GetComponent<Tree>().data);

        List<TreeInstance> tList = new List<TreeInstance>();

        tList.Add(treeInstance);

        treeInstance.position = new Vector3(Random.value, 0, Random.value);

        treeInstance.rotation = Random.value * Mathf.PI * 2;

        tList.Add(treeInstance);

        terrain.terrainData.treeInstances = tList.ToArray();

        // print( treePrototype );
        //var td ScriptableObject.CreateInstance(System.da)
	}
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A)) {
          

        }
	}
}
