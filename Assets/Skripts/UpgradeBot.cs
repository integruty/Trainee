using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBot : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform UpItemPrefab;
    public RectTransform content;

    public class UpgradeItemModel
    {
        public string name, description;
        public GameObject itemFab;
    }


}
