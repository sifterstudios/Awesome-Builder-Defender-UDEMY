using Building;
using UnityEngine;

namespace Resource
{
    public class ResourceGenerator : MonoBehaviour
    {
        ResourceGeneratorData _resourceGeneratorData;
        float _timer;
        float _timerMax;

        void Awake()
        {
            _resourceGeneratorData = GetComponent<BuildingTypeHolder>().buildingType.resourceGeneratorData;
            _timerMax = _resourceGeneratorData.timerMax;
        }

        void Start()
        {
            var nearbyResourceAmount = GetNearbyResourceAmount(_resourceGeneratorData, transform.position);

            if (nearbyResourceAmount == 0)
                // NO resource nodes nearby
                // Disable resource generator
                enabled = false;
            else
                _timerMax = _resourceGeneratorData.timerMax / 2f + _resourceGeneratorData.timerMax *
                    (1 - (float) nearbyResourceAmount / _resourceGeneratorData.maxResourceAmount);
        }

        void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer += _timerMax;
                ResourceManager.Instance.AddResource(_resourceGeneratorData.resourceType, 1);
            }
        }

        public static int GetNearbyResourceAmount(ResourceGeneratorData resourceGeneratorData, Vector3 position)
        {
            var collider2DArray =
                Physics2D.OverlapCircleAll(position, resourceGeneratorData.resourceDetectionRadius);

            var nearbyResourceAmount = 0;

            foreach (var collider2D in collider2DArray)
            {
                var resourceNode = collider2D.GetComponent<ResourceNode>();
                if (resourceNode != null)
                    // It's a resource node!
                    if (resourceNode.resourceType == resourceGeneratorData.resourceType)
                        // Same type!
                        nearbyResourceAmount++;
            }

            nearbyResourceAmount = Mathf.Clamp(nearbyResourceAmount, 0, resourceGeneratorData.maxResourceAmount);
            return nearbyResourceAmount;
        }

        public ResourceGeneratorData GetResourceGeneratorData()
        {
            return _resourceGeneratorData;
        }

        public float GetTimerNormalized()
        {
            return _timer / _timerMax;
        }

        public float GetAmountGeneratedPerSecond()
        {
            return 1 / _timerMax;
        }
    }
}