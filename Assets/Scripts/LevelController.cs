using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    #region --- Serialize Fields ---

    [SerializeField] private string nextLevelName;

    #endregion Serialize Fields
    
    
    #region --- Members ---

    private EnemyController[] _enemy;
    private bool _allEnemiesDead;

    #endregion Members


    #region --- Mono Methods ---

    private void OnEnable()
    {
        _enemy = FindObjectsOfType<EnemyController>();
    }

    private void Update()
    {
        if (!IsAllEnemiesDead() || !_allEnemiesDead) return;
        
        _allEnemiesDead = false;
        HandleNextLevel();
    }

    #endregion Mono Methods


    #region --- Private Methods ---

    private bool IsAllEnemiesDead()
    {
        foreach (EnemyController enemy in _enemy)
        {
            if (enemy.gameObject.activeSelf)
            {
                return _allEnemiesDead = false;
            }
        }

        return _allEnemiesDead = true;
    }

    private void HandleNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 9)
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
            
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    #endregion Private Methods
}
