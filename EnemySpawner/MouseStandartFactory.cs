using TMPro;
using UnityEngine;

public class MouseStandartFactory : MouseAbstractFactory
{
    private Transform _spawnPoint;
    private MouseController _mouseController;

    public MouseStandartFactory(Transform spawnPoint )
    {
        _spawnPoint = spawnPoint;
    }

    public override GameObject CreateMouseStandard()
    {
        var _standardMousePrefab = Resources.Load<GameObject>(path: "MouseStandardEnemy/MouseHelper");
        var standardMouse = UnityEngine.Object.Instantiate(_standardMousePrefab, _spawnPoint.position, Quaternion.identity);
        //var standardMouse = UnityEngine.Object.Instantiate(_standardMousePrefab, _spawnPoint.position, Quaternion.identity);
        return standardMouse;
    }

    public override GameObject CreateMouseModify()
    {
        var _modernizedMousePrefab = Resources.Load<GameObject>(path: "MouseStandardEnemy/MouseModify");
        //var modernizedMouse = UnityEngine.Object.Instantiate(_modernizedMousePrefab, _spawnPoint.position, Quaternion.identity);
        var modernizedMouse = UnityEngine.Object.Instantiate(_modernizedMousePrefab, _spawnPoint.position, Quaternion.identity);
        return modernizedMouse;
    }
}
