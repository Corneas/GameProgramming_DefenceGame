using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    private float constructionTimer;
    private float constructionTimerMax;

    private BuildingTypeSO buildingType;

    private BoxCollider2D boxCollider;

    private SpriteRenderer spriteRenderer;
    private BuildingTypeHolder buildingTypeHolder;

    private Material constructionMaterial;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        constructionMaterial = spriteRenderer.material;
    }

    private void Update()
    {
        constructionTimer -= Time.deltaTime;

        constructionMaterial.SetFloat("_Prgress", GetConstructionTimerNormalized());

        if(constructionTimer <= 0f)
        {
            Instantiate(buildingType.prefab, transform.position, Quaternion.identity);
            Instantiate(GameAssets.Instance.pfBuildingPlacedParticles, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
            Destroy(gameObject);
        }
    }

    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingType)
    {
        Debug.Log("����");
        Transform pfBuildingConstruction = Instantiate(GameAssets.Instance.pfBuildingConstruction, position, Quaternion.identity);
        //Transform buildingConstructionTransform = Instantiate(pfBuildingConstruction, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = pfBuildingConstruction.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(buildingType);

        return buildingConstruction;
    }

    private void SetBuildingType(BuildingTypeSO buildingType)
    {
        this.buildingType = buildingType;

        buildingTypeHolder.buildingType = buildingType;

        constructionTimerMax = buildingType.constructionTimerMax;
        constructionTimer = constructionTimerMax;

        spriteRenderer.sprite = buildingType.sprite;

        boxCollider.offset = buildingType.prefab.GetComponent<BoxCollider2D>().offset;
        boxCollider.size = buildingType.prefab.GetComponent<BoxCollider2D>().size;

    }

    public float GetConstructionTimerNormalized()
    {
        return 1 - constructionTimer / constructionTimerMax;
    }
}
