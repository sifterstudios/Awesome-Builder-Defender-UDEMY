using System;
using System.Collections.Generic;
using BD.Building;
using BD.Building.SO;
using BD.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace BD.UI
{
    public class BuildingTypeSelectUI : MonoBehaviour
    {
        [SerializeField] Sprite arrowSprite;
        [SerializeField] List<BuildingTypeSO> ignoreBuildingTypeList;
        Dictionary<BuildingTypeSO, Transform> _btnTransformDictionary;
        Transform _arrowBtn;

        void Awake()
        {
            Transform btnTemplate = transform.Find("btnTemplate");
            btnTemplate.gameObject.SetActive(false);
            _btnTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();
            BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(nameof(BuildingTypeListSO));

            int index = 0;
            _arrowBtn = Instantiate(btnTemplate, transform);
            _arrowBtn.gameObject.SetActive(true);

            float offsetAmount = 130f;
            _arrowBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            _arrowBtn.Find("image").GetComponent<Image>().sprite = arrowSprite;
            _arrowBtn.Find("image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30);


            _arrowBtn.GetComponent<Button>().onClick
                .AddListener(() => BuildingManager.Instance.SetActiveBuildingType(null));

            MouseEnterExitEvents mouseEnterExitEvents = _arrowBtn.GetComponent<MouseEnterExitEvents>();
            mouseEnterExitEvents.OnMouseEnter += (sender, e) => { TooltipUI.Instance.Show("Arrow"); };
            mouseEnterExitEvents.OnMouseExit += (sender, e) => { TooltipUI.Instance.Hide(); };


            index++;

            foreach (BuildingTypeSO buildingType in buildingTypeList.list)
            {
                if (ignoreBuildingTypeList.Contains(buildingType)) continue;
                Transform btnTransform = Instantiate(btnTemplate, transform);
                btnTransform.gameObject.SetActive(true);

                offsetAmount = 130f;
                btnTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

                btnTransform.Find("image").GetComponent<Image>().sprite = buildingType.sprite;

                btnTransform.GetComponent<Button>().onClick
                    .AddListener(() => BuildingManager.Instance.SetActiveBuildingType(buildingType));

                mouseEnterExitEvents = btnTransform.GetComponent<MouseEnterExitEvents>();
                mouseEnterExitEvents.OnMouseEnter += (sender, e) =>
                {
                    TooltipUI.Instance.Show(buildingType.nameString + "\n" +
                                            buildingType.GetConstructionResourceCostString());
                };
                mouseEnterExitEvents.OnMouseExit += (object sender, EventArgs e) => {
                    TooltipUI.Instance.Hide();
                };
                _btnTransformDictionary[buildingType] = btnTransform;

                index++;
            }
        }

        void Start()
        {
            BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
            UpdateActiveBuildingTypeButton();
        }

        void BuildingManager_OnActiveBuildingTypeChanged(object sender,
            BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
        {
            UpdateActiveBuildingTypeButton();
        }


        void UpdateActiveBuildingTypeButton()
        {
            _arrowBtn.Find("selected").gameObject.SetActive(false);
            foreach (BuildingTypeSO buildingType in _btnTransformDictionary.Keys)
            {
                Transform btnTransform = _btnTransformDictionary[buildingType];
                btnTransform.Find("selected").gameObject.SetActive(false);
            }

            BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
            if (activeBuildingType == null)
            {
                _arrowBtn.Find("selected").gameObject.SetActive(true);
            }
            else
            {
                _btnTransformDictionary[activeBuildingType].Find("selected").gameObject.SetActive(true);
            }
        }
    }
}