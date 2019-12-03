//Copyright (c) 2017 Kai Clavier [kaiclavier.com] Do Not Distribute
using UnityEngine;
using System.Collections;

public class STMContinue : MonoBehaviour {
	public SuperTextMesh stm;
	public KeyCode key = KeyCode.Space;
	
	void Update () {
		if(Input.GetKeyDown(key)){
			stm.Continue();
		}
	}
}
