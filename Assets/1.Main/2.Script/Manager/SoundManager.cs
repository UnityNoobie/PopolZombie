using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : SingletonDontDestroy<SoundManager>
{
    #region PlayerSound

    public AudioClip m_handGunSound;
    public AudioClip m_SMGSound;
    public AudioClip m_RifleSound;
    public AudioClip m_ShotGunSound;
    public AudioClip m_MGSound;
    public AudioClip m_SwingSound;
    public AudioClip m_fireSound;
    public AudioClip m_reroadSound;
    public AudioClip m_playerWalk;
    public AudioClip m_playerDeath;
    public AudioClip m_playerRevive;
    public AudioClip m_playerHeal;
    public AudioClip m_playerDamaged;
    public AudioClip m_playerMeleeAtk;

    #endregion
    #region ZombieSound
    public AudioClip m_zombieAtk;
    public AudioClip m_zombieDmg;
    public AudioClip m_zombieChace;
    public AudioClip m_zombieDeath;

    public AudioClip m_bossSkill1;
    public AudioClip m_bossSkill2;
    public AudioClip m_bossAtk;
    public AudioClip m_bossDeath;
    public AudioClip m_bossChase;
    public AudioClip m_bossRage;


    #endregion
    #region SystemSound
    public AudioClip m_bgm_nightStart1;
    public AudioClip m_bgm_nightStart2;
    public AudioClip m_bgm_dayStart1;
    public AudioClip m_bgm_dayStart2;
    public AudioClip m_bgm_lobby1;
    public AudioClip m_bgm_lobby2;
    public AudioClip m_bgm_lobby3;
    public AudioClip m_bgm_day1;
    public AudioClip m_bgm_day2;
    public AudioClip m_bgm_day3;
    public AudioClip m_bgm_night;
    public AudioClip m_bgm_night2;
    public AudioClip m_bgm_night3;
    public AudioClip m_bgm_nightBoss;
    #endregion





    public AudioClip m_volumeMuteSound;

    public AudioClip m_dd;
    public AudioClip m_da;
    public AudioClip m_ds;
    public AudioClip m_df;
    public AudioClip m_dg;
    public AudioClip m_dh;


}
