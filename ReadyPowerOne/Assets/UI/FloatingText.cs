using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextBehavior : MonoBehaviour {

	public Animator animator;
	private Text animatorText{ get; set;}

	void Start()
	{
		AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo (0);
		Destroy (gameObject, clipInfo [0].clip.length);
		animatorText = animator.GetComponent<Text> ();
	}

}
