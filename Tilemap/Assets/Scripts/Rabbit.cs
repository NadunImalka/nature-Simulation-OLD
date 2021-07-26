using Pathfinding;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public float OverlapRadius = 10.0f;
    public float speed = 0.5f;
    
    
    private bool atTheTarget, busy;
    private bool noTargetsInRange;
    private Transform Player;
    private Transform target;
    private int targetLayer;
    private string task;
    private float thirst = 100f, hunger = 101f;
    private Collider roamPoint;
    private int roamLayer;
    private float eatDrinkSpeed = 0.2f;
    


    private void Start()
    {
        Player = transform;
        targetLayer = LayerMask.NameToLayer("Water");
        roamLayer = LayerMask.NameToLayer("roamPoints");
        //Debug.Log(LayerMask.LayerToName(targetLayer));
    }

    private void Update()
    {
        if (target != null)
            atTheTarget = Vector3.Distance(transform.position, target.position) < 3f;
        if (target == null) atTheTarget = false;

        //Debug.Log(LayerMask.LayerToName(targetLayer));
        Debug.Log(hunger + " " + thirst + "     " + task + "  " + busy + atTheTarget);
    }

    private void FixedUpdate()
    {
        ChooseTask();
        hunger += 0.01f;
        thirst += 0.01f;
        Vision();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, OverlapRadius);
    }

    private void ChooseTask()
    {
        if (task == "freeRoam") busy = false;
        if (hunger > thirst && !busy)
        {
            Debug.Log("Set to free roam in food");
            task = "food";
            if (hunger<10)
            {
                target = null;
            }
        }
        else if (thirst > hunger && !busy)
        {
            Debug.Log("Set to free roam in water");
            task = "water"; 
            if (thirst<10)
            {
                target = null;
            }
        }
        
        targetLayer = LayerMask.NameToLayer(task);
        
        if (target != null)
        {
            gameObject.GetComponent<AIDestinationSetter>().target = target;
            //gameObject.GetComponent<AIPath>().enabled = true;
            DoTask();
            GetComponent<AIPath>().maxSpeed = 3f;
        }
        else
        {
            FreeRoam();
            Debug.Log("FREEEEE");
            GetComponent<AIPath>().maxSpeed = 2f;
        }

        
    }

    private void DoTask()
    {
        busy = atTheTarget;
        if (atTheTarget)
        {
            if (target == null) busy = false;
            Debug.Log("                      ssssssssssss                 sssssssssss" + task + "  " +
                      target.gameObject.name + "  "
                      + LayerMask.LayerToName(targetLayer));
            if (task == "water" && target.gameObject.name == "water")
            {
                thirst -= eatDrinkSpeed;
                if (thirst < 1) busy = false;
            }
            
            if (task == "food" && target.gameObject.name == "plant")
            {
                hunger -= eatDrinkSpeed;
                if (hunger < 1) busy = false;
                
                /*if (target.GetComponent<plants>().food < 2) busy = false;*/
                target.GetComponent<plants>().food -= eatDrinkSpeed;
                
                
            }
        }
    }
    
    private void FreeRoam()
    {
        if (roamPoint==null || Vector3.Distance(roamPoint.transform.position , Player.position)<3f)
        {
            Collider[] roamPoints = Physics.OverlapSphere(Player.position, OverlapRadius*5, 1 << roamLayer);
            int randomPoint = Random.Range(0, roamPoints.Length);
            roamPoint = roamPoints[randomPoint];
            roamPoint.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        GetComponent<AIDestinationSetter>().target = roamPoint.transform;
    }

    private void Vision()
    {
        Debug.LogWarning(LayerMask.LayerToName(targetLayer));
        if (target != null) target.GetComponent<MeshRenderer>().material.color = Color.green;

        var hitColliders = Physics.OverlapSphere(Player.position, OverlapRadius, 1 << targetLayer);
        var minimumDistance = Mathf.Infinity;
        foreach (var collider in hitColliders)
        {
            var distance = Vector3.Distance(Player.position, collider.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                target = collider.transform;
            }
        }

        //if (target != null && target.gameObject.layer != LayerMask.NameToLayer(task)) target = null;
        if (target != null)
        {
            target.GetComponent<MeshRenderer>().material.color = Color.red;
            noTargetsInRange = false;
            Debug.Log("Nearest Enemy: " + target + "; Distance: " + minimumDistance);
        }
        else
        {
            Debug.Log("There is no enemy in the given radius");
            noTargetsInRange = true;
        }
    }

    /*private void movement()
    {
        //Debug.Log("Distance" + Vector3.Distance(transform.position,target.position));
        if (Vector3.Distance(transform.position, target.position) > 2f)
        {
            transform.Translate(new Vector3(target.position.x - transform.position.x, 0f,
                target.position.z - transform.position.z) * (speed * Time.deltaTime));
        }

        atTheTarget = Vector3.Distance(transform.position, target.position) < 2.1f ? true : false;
    }*/
}