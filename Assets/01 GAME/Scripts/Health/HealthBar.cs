using System;
using UnityEngine;

namespace BD.Health
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] HealthSystem healthSystem;

        Transform _barTransform;
        Transform _separatorCointainer;

        void Awake()
        {
            _barTransform = transform.Find("bar");
        }

        void Start()
        {
            _separatorCointainer = transform.Find("separatorCointainer");

            ConstructHealthBarSeparators();

            healthSystem.OnDamaged += HealthSystem_OnDamaged;
            healthSystem.OnHealed += HealthSystem_OnHealed;
            healthSystem.OnHealthAmountMaxChanged += (sender, args) => ConstructHealthBarSeparators();


            UpdateBar();
            UpdateHealthBarVisible();
        }

        void ConstructHealthBarSeparators()
        {
            Transform separatorTemplate = _separatorCointainer.Find("separatorTemplate");
            separatorTemplate.gameObject.SetActive(false);
            foreach (Transform separatorTransform in _separatorCointainer)
            {
                if (separatorTransform == separatorTemplate) continue;
                Destroy(separatorTransform.gameObject);
            }

            int healthAmountPerSeparator = 10;
            float barSize = 3f;
            float barOneHealthAmountSize = barSize / healthSystem.GetHealthAmountMax();
            int healthSeparatorCount = Mathf.FloorToInt(healthSystem.GetHealthAmountMax() / healthAmountPerSeparator);

            for (int i = 1; i < healthSeparatorCount; i++)
            {
                Transform separatorTransform = Instantiate(separatorTemplate, _separatorCointainer);
                separatorTransform.gameObject.SetActive(true);
                separatorTransform.localPosition = new Vector3(barOneHealthAmountSize * i * 10, 0, 0);
            }

            separatorTemplate.gameObject.SetActive(false);
        }

        void HealthSystem_OnHealed(object sender, EventArgs e)
        {
            UpdateBar();
            UpdateHealthBarVisible();
        }

        void HealthSystem_OnDamaged(object sender, EventArgs e)
        {
            UpdateBar();
            UpdateHealthBarVisible();
        }

        void UpdateBar()
        {
            _barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
        }

        void UpdateHealthBarVisible()
        {
            if (healthSystem.IsFullHealth())
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }
}