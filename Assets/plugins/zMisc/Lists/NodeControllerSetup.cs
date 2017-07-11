using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NodeControllerSetup
{
    [Header("use to rescan")]
    public bool rescanTemplates;
    [SerializeField]
    public List<zNode> nodeTemplatePool;

    public Image image;
    public Text text;
    [Header("Mask")]
    public RectTransform contentMaskRect;
    [Header("Traditionally {Templates}")]
    public GameObject templatePoolGO;
    [Header("Will be spawned on start")]
    public RectTransform content;
    [Header("Colors")]
    public Color nonHoveredColor = new Color(1, 1, 1, 0.2f);
    public Color hoveredColor = new Color(1, 1, 1, 0.4f);
    public Color activeColor = new Color(1, 0, 0, 0.3f);
    public Color panelEditColor = new Color(.5f, 0, 0, 0.1f);
}
