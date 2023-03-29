using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    
    #region SkillTypeChecker
    public bool isActiveType(SkillWeaaponType skillweapon, WeaponType weaponType)
    {
        switch (skillweapon)
        {
            case SkillWeaaponType.Every: return true;
            case SkillWeaaponType.Personal:
                if (weaponType.Equals(WeaponType.Pistol) || weaponType.Equals(WeaponType.SubMGun) || weaponType.Equals(WeaponType.Rifle)) return true;
                else return false;
            case SkillWeaaponType.Heavy:
                if (weaponType.Equals(WeaponType.Bat) || weaponType.Equals(WeaponType.Axe) || weaponType.Equals(WeaponType.MachineGun) || weaponType.Equals(WeaponType.ShotGun)) return true;
                else return false;
            case SkillWeaaponType.Pistol:
                if (weaponType.Equals(WeaponType.Pistol)) return true;
                else return false;
            case SkillWeaaponType.SMG:
                if (weaponType.Equals(WeaponType.SubMGun)) return true;
                else return false;
            case SkillWeaaponType.Rifle:
                if (weaponType.Equals(WeaponType.Rifle)) return true;
                else return false;
            case SkillWeaaponType.MachineGun:
                if (weaponType.Equals(WeaponType.MachineGun)) return true;
                else return false;
            case SkillWeaaponType.ShotGun:
                if (weaponType.Equals(WeaponType.ShotGun)) return true;
                else return false;
            case SkillWeaaponType.Melee:
                if (weaponType.Equals(WeaponType.Bat) || weaponType.Equals(WeaponType.Axe)) return true;
                else return false;
        }
        return false;
    }
    #endregion
    WeaponType m_weapontype;
    SkillWeaaponType m_skillweapon;
    int m_skillPoint = 0;
    int SPCount = 0;
    List<int> m_skillList = new List<int>();
    public void LevelUP(WeaponType weapontype)
    {
        m_weapontype = weapontype;
        if(SPCount < 30)
        {
            m_skillPoint++;
            SPCount++;
        }
    }
    public void ActiveSkill()
    {

    }



}
