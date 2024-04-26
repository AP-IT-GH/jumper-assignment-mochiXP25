using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MLPlayer : Agent
{
    public float Force = 15f;
    private Rigidbody rb = null;
    public Transform orig = null;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    /*    public override void OnActionReceived(ActionBuffers actionBuffers)
        {
            var discreteActions = actionBuffers.DiscreteActions;
            if (discreteActions[0] == 1)
            {
                Thrust();
            }
            var continuousActions = actionBuffers.ContinuousActions;
            if (continuousActions[0] > 0.5f) // Assuming continuousActions[0] controls the jump action
            {
                Thrust();
            }
        }*/
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        var discreteActions = actionBuffers.DiscreteActions;
        if (discreteActions.Length > 0 && discreteActions[0] == 1)
        {
            Thrust();
        }
    }

    public override void OnEpisodeBegin()
    {
        ResetPlayer();
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;
/*        if (Input.GetKey(KeyCode.UpArrow))
        {
            discreteActionsOut[0] = 1;
        }*/
        if (Input.GetAxis("Vertical") > 0) // Assuming "Vertical" axis controls jumping
        {
            discreteActionsOut[0] = 1;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("obstacle") == true)
        {
            AddReward(-1.0f);
            Destroy(collision.gameObject);
            if (collision.gameObject.CompareTag("road")==true)
            {
                AddReward(-1.0f);
                EndEpisode();
            }
            EndEpisode();
        }
        if (collision.gameObject.CompareTag("walltop") == true)
        {
            AddReward(-1.0f);
            EndEpisode() ;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wallend")==true)
        {
            AddReward(1.0f);
            EndEpisode();
        }
    }
    private void Thrust()
    {
        rb.AddForce(Vector3.up * Force, ForceMode.Acceleration);
    }

    private void ResetPlayer()
    {
        transform.position = orig.position;
    }
}
