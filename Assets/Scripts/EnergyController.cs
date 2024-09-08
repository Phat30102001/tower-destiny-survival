using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public EnergyData energyData;

    private float currentEnergy;

    public float energyGenerateInterval = 1.0f;



    public IEnumerator<float> GenerateEnergy()
    {
        while (true)
        {

            currentEnergy += energyData.EnergyGenerateValuePerSecond * energyGenerateInterval;
            Debug.Log("Energy Generated: " + currentEnergy);

            yield return Timing.WaitForSeconds(energyGenerateInterval);
        }
    }

    public bool ConsumeEnergy(float amount)
    {
        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }


    public void SetEnergyData(EnergyData _data)
    {
        currentEnergy = 0;
        energyData.Level = _data.Level;
        energyData.EnergyGenerateValuePerSecond = _data.EnergyGenerateValuePerSecond;
    }
}

[Serializable]
public struct EnergyData
{
    public int Level;
    public ResourceData Price;
    public float EnergyGenerateValuePerSecond;
}
