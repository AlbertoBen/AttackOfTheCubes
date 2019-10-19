using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "WeaponParameters", order = 1)]
public class WeaponParameters : ScriptableObject
{
	public string mName= "Weapon";
	public float mCadence=1;
	public int mProyectiles = 10;
	public float mSpreadAngle = 5;
	public float mProyectileSpeed = 50;
	public int mDamagePerProyectile = 10;
}
