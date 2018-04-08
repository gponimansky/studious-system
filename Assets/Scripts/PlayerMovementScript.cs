using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour {

    public Animator anim;
    int jumpHash = Animator.StringToHash("Jump");
    int fireHash = Animator.StringToHash("Fire");
    int idleStateHash = Animator.StringToHash("Base Layer.Movement");
	
	// Update is called once per frame
	void Update () {
        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
            z *= 2;

        anim.SetFloat("Vertical", z);
        anim.SetFloat("Horizontal", x);

        z *= Time.deltaTime * 3.0f;
        x *= Time.deltaTime * 150.0f;

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (Input.GetKeyDown(KeyCode.Space) && stateInfo.fullPathHash == idleStateHash)
            anim.SetTrigger(jumpHash);

        if (Input.GetMouseButtonDown(0) && stateInfo.fullPathHash == idleStateHash)
            anim.SetTrigger(fireHash);

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }
}
