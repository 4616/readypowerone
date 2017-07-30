using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UpgradePanelBehavior : MonoBehaviour {

	public GameObject UpgradeOptions;
	public GameObject UpgradeButtonPrefab;

	public int kUpgrades;

	public List<Upgrade> Upgrades;

	// Use this for initialization
	void Start () {
		Upgrades = ChooseUpgrades ();

		foreach (Upgrade u in Upgrades){
			GameObject Btn = Instantiate (UpgradeButtonPrefab, UpgradeOptions.transform);
			Btn.GetComponent<UpgradeButtonBehavior> ().makeButton(u);
		}

	}

	// Update is called once per frame
	void Update () {
		
	}

	private List<Upgrade> ChooseUpgrades(){
		// sample k upgrades with resevoir sampling
		var samples = new List<Upgrade>();
		int n = 0;

		foreach(Upgrade u in Upgrade.upgrades)
		{
			n++;
			if (samples.Count < kUpgrades)
			{
				samples.Add(u);
			}
			else
			{
				int s = Random.Range(0,n);
				if(s < kUpgrades)
				{
					samples[s] = u;
				}
			}
		}
		return samples;
	}
}
