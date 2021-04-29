using System;
using System.Collections.Generic;
using BD.Resource;
using BD.Resource.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BD.UI
{
    public class ResourcesUI : MonoBehaviour
    {
        ResourceTypeListSO _resourceTypeList;
        Dictionary<ResourceTypeSO, Transform> _resourceTypeTransformDictionary;

        void Awake()
        {
            _resourceTypeList = Resources.Load<ResourceTypeListSO>(nameof(ResourceTypeListSO));

            _resourceTypeTransformDictionary = new Dictionary<ResourceTypeSO, Transform>();

            Transform resourceTemplate = transform.Find("resourceTemplate");
            resourceTemplate.gameObject.SetActive(false);

            int index = 0;
            foreach (ResourceTypeSO resourceType in _resourceTypeList.list)
            {
                // Create new gameobjects
                Transform resourceTransform = Instantiate(resourceTemplate, transform);
                resourceTransform.gameObject.SetActive(true);

                // Place gameobjects
                float offsetAmount = -160f;
                resourceTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

                // Get the correct images, from a field in the scriptable objects!
                resourceTransform.Find("image").GetComponent<Image>().sprite = resourceType.sprite;

                // Cache the information to bring into other method. Needs to be called after Start()
                _resourceTypeTransformDictionary[resourceType] = resourceTransform;
                index++;
            }
        }

        void Start()
        {
            ResourceManager.Instance.OnResourceAmountChanged += ResourceManager_OnResourceAmountChanged;
            UpdateResourceAmount();
        }

        void ResourceManager_OnResourceAmountChanged(object sender, EventArgs e)
        {
            UpdateResourceAmount();
        }

        void UpdateResourceAmount()
        {
            foreach (ResourceTypeSO resourceType in _resourceTypeList.list)
            {
                // Get info from the dictionary/cache
                Transform resourceTransform = _resourceTypeTransformDictionary[resourceType];
                // Get resource amount from ResourceManager Script
                int resourceAmount = ResourceManager.Instance.GetResourceAmount(resourceType);

                // Update text to match!
                resourceTransform.Find("text").GetComponent<TextMeshProUGUI>().SetText(resourceAmount.ToString());
            }
        }
    }
}
