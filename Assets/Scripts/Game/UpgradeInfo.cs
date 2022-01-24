using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeInfo : MonoBehaviour
{
    static Dictionary<DiceBase, int> upgradeinfo;
    public static UpgradeInfo i { get; set; }
    public Dictionary<DiceBase, int> Upgradeinfo { get => upgradeinfo; }

    private void Awake()
    {
        i = this;
        upgradeinfo = new Dictionary<DiceBase, int>();
        var DiceList = Resources.LoadAll<DiceBase>("");
        foreach (var d in DiceList)
        {
            if(upgradeinfo.ContainsKey(d))
            {
                Debug.LogError("error");
            }
            upgradeinfo[d] = 0;
        }
    }
    public void Init()
    {
        upgradeinfo.Keys.ToList().ForEach(key =>
        {
            upgradeinfo[key] = 0;
        });
    }
}
