using UnityEngine;
using System.Collections;
using System.ComponentModel;
using Steamworks;

// This is a port of StatsAndAchievements.cpp from SpaceWar, the official Steamworks Example.
class SteamStatsAndAchievements : MonoBehaviour {
	private enum Achievement : int {
		LEVEL_ONE,
		FIVE_SECONDS,
		TEN_SECONDS,
	};

	private Achievement_t[] m_Achievements = new Achievement_t[] {
		new Achievement_t(Achievement.LEVEL_ONE, "Bronze Rocket", "You completed the first level!"),
		new Achievement_t(Achievement.FIVE_SECONDS, "Silver Rocket", "You survived 5 seconds on all levels!"),
		new Achievement_t(Achievement.TEN_SECONDS, "Gold Rocket", "You survived 10 seconds on all levels!"),
	};

	// Our GameID
	private CGameID m_GameID;

	protected Callback<UserStatsReceived_t> m_UserStatsReceived;
	protected Callback<UserAchievementStored_t> m_UserAchievementStored;

	void OnEnable() {
		if (!SteamManager.Initialized)
			return;

		// Cache the GameID for use in the Callbacks
		m_GameID = new CGameID(SteamUtils.GetAppID());

		m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
		m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
	}

	private void Update() {
		if (!SteamManager.Initialized)
			return;

		// Get info from sources

		// Evaluate achievements
		foreach (Achievement_t achievement in m_Achievements) {
			if (achievement.m_bAchieved)
				continue;

			switch (achievement.m_eAchievementID) {
				case Achievement.LEVEL_ONE:
					if (GameManager.Instance.bestTimes != null && GameManager.Instance.bestTimes[0,0] >= 5f) {
						UnlockAchievement(achievement);
					}
					break;
				case Achievement.FIVE_SECONDS:
					if (CheckAllAboveFloat(GameManager.Instance.bestTimes, 5f)) {
						UnlockAchievement(achievement);
					}
					break;
				case Achievement.TEN_SECONDS:
					if (CheckAllAboveFloat(GameManager.Instance.bestTimes, 10f)) {
						UnlockAchievement(achievement);
					}
					break;
			}
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: Unlock this achievement
	//-----------------------------------------------------------------------------
	private void UnlockAchievement(Achievement_t achievement) {
		achievement.m_bAchieved = true;

		// the icon may change once it's unlocked
		//achievement.m_iIconImage = 0;

		// mark it down
		SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());
	}
	
	//-----------------------------------------------------------------------------
	// Purpose: We have stats data from Steam. It is authoritative, so update
	//			our data with those results now.
	//-----------------------------------------------------------------------------
	private void OnUserStatsReceived(UserStatsReceived_t pCallback) {
		if (!SteamManager.Initialized)
			return;

		// we may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID) {
			if (EResult.k_EResultOK == pCallback.m_eResult) {
				Debug.Log("Received achievements from Steam\n");

				// load achievements
				foreach (Achievement_t ach in m_Achievements) {
					bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);
					if (ret) {
						ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
						ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
					}
					else {
						Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
					}
				}
			}
			else {
				Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
			}
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: An achievement was stored
	//-----------------------------------------------------------------------------
	private void OnAchievementStored(UserAchievementStored_t pCallback) {
		// We may get callbacks for other games' stats arriving, ignore them
		if ((ulong)m_GameID == pCallback.m_nGameID) {
				Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
		}
	}

	//-----------------------------------------------------------------------------
	// Purpose: Display the user's achievements
	//-----------------------------------------------------------------------------
	private class Achievement_t {
		public Achievement m_eAchievementID;
		public string m_strName;
		public string m_strDescription;
		public bool m_bAchieved;

		/// <summary>
		/// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
		/// </summary>
		/// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
		/// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
		/// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
		public Achievement_t(Achievement achievementID, string name, string desc) {
			m_eAchievementID = achievementID;
			m_strName = name;
			m_strDescription = desc;
			m_bAchieved = false;
		}
	}


    static bool CheckAllAboveFloat(float[,] array, float check)
    {
		if(array == null)
		{
			return false;
		}
        // Get the dimensions of the array
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);

        // Iterate through each element and check if it's above 5
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (array[i, j] < check)
                {
                    // If any element is not above 5 or equal to, return false
                    return false;
                }
            }
        }

        // If all elements are above 5, return true
        return true;
    }
}