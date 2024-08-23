public interface IWeapon
{
    public void SetData(WeaponBaseData _data)
    {

    }
}
public class WeaponBaseData
{
    public string WeaponId;
    public int DamageAmount;
    public float Cooldown;
    public int NumberPerRound;
    public float FireOffset;
    public string TargetTag;
}
