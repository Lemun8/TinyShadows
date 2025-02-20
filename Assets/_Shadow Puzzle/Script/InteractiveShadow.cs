using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractiveShadow : MonoBehaviour
{
    public Transform shadowMiddlePoint;

    [SerializeField] private Transform shadowTransform;

    [SerializeField] private Transform lightTransform;
    private LightType lightType;

    [SerializeField] private GameObject[] allLight;
    private GameObject nearestObject;
    float distance;

    [SerializeField] private LayerMask targetLayerMask;

    [SerializeField] private Vector3 extrusionDirection = Vector3.zero;

    private Vector3[] objectVertices;

    private Mesh shadowColliderMesh;
    private MeshCollider shadowCollider;

    private Vector3 previousPosition;
    private Quaternion previousRotation;
    private Vector3 previousScale;

    private Vector3 lightPreviousPosition;
    private Vector3 lightPreviousRotation;
    private Vector3 lightPreviousScale;

    private bool canUpdateCollider = true;
    public Transform ShadowTransfrom => shadowTransform;

    // Reference to the BoxPull component
    private BoxPull boxPull;

    [SerializeField][Range(0.02f, 1f)] private float shadowColliderUpdateTime = 0.08f;

    private void Awake()
    {
        allLight = GameObject.FindGameObjectsWithTag("Light");
        InitializeShadowCollider();

        lightType = lightTransform.GetComponent<Light>().type;

        objectVertices = transform.GetComponent<MeshFilter>().mesh.vertices.Distinct().ToArray();

        shadowColliderMesh = new Mesh();

        boxPull = GetComponent<BoxPull>();
        if (boxPull == null)
        {
            Debug.LogWarning("BoxPull component not found on this object.");
        }
        //shadowColliderMesh = new Mesh();
        //shadowCollider.sharedMesh = shadowColliderMesh;
    }

    private void Update()
    {
        shadowTransform.position = transform.position;
        if(shadowMiddlePoint!= null )
        {
            shadowMiddlePoint.position = shadowTransform.GetComponent<MeshCollider>().bounds.center;
        }

        // Enable or disable the shadow collider based on beingPushed
        if (boxPull != null && shadowCollider != null)
        {
            shadowCollider.enabled = !boxPull.beingPushed;
        }
    }

    private void FixedUpdate()
    {
        if (TransformHasChanged() && canUpdateCollider)
        {
            Invoke("UpdateShadowCollider", shadowColliderUpdateTime);
            NearestLight();
            canUpdateCollider = false;
        }

        if(LightTransformHasChanged() && canUpdateCollider)
        {
            Invoke("UpdateShadowCollider", shadowColliderUpdateTime);
            NearestLight();
            canUpdateCollider = false;
        }

        previousPosition = transform.position;
        previousRotation = transform.rotation;
        previousScale = transform.localScale;
    }

    private void InitializeShadowCollider()
    {
        GameObject shadowGameObject = shadowTransform.gameObject;
        //shadowGameObject.hideFlags = HideFlags.HideInHierarchy; //OPTIONNAL
        shadowCollider = shadowGameObject.AddComponent<MeshCollider>();
        shadowCollider.convex = true;
        shadowCollider.isTrigger = true;
    }

    private void UpdateShadowCollider()
    {
        shadowColliderMesh.vertices = ComputeShadowColliderMeshVertices();
        shadowCollider.sharedMesh = shadowColliderMesh;
        canUpdateCollider = true;
    }

    private void NearestLight()
    {
        Debug.Log("Nearest Light");
        GameObject closest = null;
        float closestDistance = float.MaxValue;

        if(allLight == null)
        {
            
            GameObject g = GameObject.FindGameObjectWithTag("Main Light");
            lightTransform = g.transform;
            lightType = lightTransform.GetComponent<Light>().type;
            Debug.Log("No light found");
            return;
        }

        foreach (GameObject light in allLight)
        {

            distance = Vector3.Distance(transform.position, light.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = light;
            }
        }

        if (closest != null && closest != nearestObject)
        {
            nearestObject = closest;
            lightTransform = nearestObject.transform;
            lightType = lightTransform.GetComponent<Light>().type;
        }
    }

    private Vector3[] ComputeShadowColliderMeshVertices()
    {
        Vector3[] points = new Vector3[2 * objectVertices.Length];

        Vector3 raycastDirection = lightTransform.forward;

        int n = objectVertices.Length;

        for (int i = 0; i < n; i++)
        {
            Vector3 point = transform.TransformPoint(objectVertices[i]);

            if (lightType != LightType.Directional)
            {
                raycastDirection = point - lightTransform.position;
            }

            points[i] = ComputeIntersectionPoint(point, raycastDirection);

            points[n + i] = ComputeExtrusionPoint(point, points[i]);
        }

        return points;
    }

    private Vector3 ComputeIntersectionPoint(Vector3 fromPosition, Vector3 direction)
    {
        RaycastHit hit;

        if (Physics.Raycast(fromPosition, direction, out hit, Mathf.Infinity, targetLayerMask))
        {
            return hit.point - transform.position;
        }
        Debug.Log("No hit detected within layer mask. - "+name);
        //Debug.DrawLine(this.gameObject,hit.transform.position);
        return fromPosition + 100 * direction - transform.position;
    }

    private Vector3 ComputeExtrusionPoint(Vector3 objectVertexPosition, Vector3 shadowPointPosition)
    {
        if (extrusionDirection.sqrMagnitude == 0)
        {
            return objectVertexPosition - transform.position;
        }
        return shadowPointPosition + extrusionDirection;
    }

    private bool TransformHasChanged()
    {
        return previousPosition != transform.position || previousRotation != transform.rotation || previousScale != transform.localScale;
    }
    private bool LightTransformHasChanged()
    {
        return previousPosition != lightTransform.position || previousRotation != lightTransform.rotation || previousScale != lightTransform.localScale;
    }
}
