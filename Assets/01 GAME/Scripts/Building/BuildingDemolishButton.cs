using BD.Building.SO;
using BD.Resource;
using UnityEngine;
using UnityEngine.UI;

namespace BD.Building
{
    public class BuildingDemolishButton : MonoBehaviour
    {
        [SerializeField] Building building;

        void Awake()
        {
            transform.Find("button").GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingTypeSO buildingType = building.GetComponent<BuildingTypeHolder>().buildingType;
                foreach (ResourceAmount resourceAmount in buildingType.constructionResourceCostArray)
                {
                    ResourceManager.Instance.AddResource(resourceAmount.resourceType,
                        Mathf.FloorToInt((resourceAmount.amount * .6f)));
                    Destroy(building.gameObject);
                }
            });
        }
    }
}