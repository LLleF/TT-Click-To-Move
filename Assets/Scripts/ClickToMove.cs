using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;




public class ClickToMove : MonoBehaviour
{
    [SerializeField] private Slider _sliderMoveSpeed;
    private LineRenderer _ropeRenderer;
    private List<Vector2> _newCoordinatesOfClick = new List<Vector2>();
    private float _speedOfSphere = 1;


    void Start()
    {
        _ropeRenderer = GetComponent<LineRenderer>();
        _newCoordinatesOfClick.Add(transform.position);
        ChangeSphereSpeedMove();
    }

    void Update()
    {

        NewCoordinates();
        MoveSphere();
    }

    //Метод получения новых координат и отрисовки линии
    private void NewCoordinates()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Проверка нажатия не на элементы интерфейса
            bool _checkPushOnUI;
#if UNITY_EDITOR
            _checkPushOnUI = !EventSystem.current.IsPointerOverGameObject();
#else
        _checkPushOnUI = Input.touchCount > 0 && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif

            if (_checkPushOnUI)
            {
                //Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _newCoordinatesOfClick.Add(worldPosition);

                DrawLine();
            }
        }
    }

    //Метод отрисовки линии
    private void DrawLine()
    {
        _ropeRenderer.positionCount = _newCoordinatesOfClick.Count;
        int IndexLastElement = _newCoordinatesOfClick.Count - 1;
        _ropeRenderer.SetPosition(IndexLastElement, _newCoordinatesOfClick[IndexLastElement]);
    }

    //Метод перемещения сферы, отрисовки границы линии, очистки листа
    private void MoveSphere()
    {
        if (_newCoordinatesOfClick.Count > 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, _newCoordinatesOfClick[1], _speedOfSphere * Time.deltaTime);
            _ropeRenderer.SetPosition(0, transform.position);

            DeleteNextCoordinate();
        }
    }

    //Метод очистки листа
    private void DeleteNextCoordinate()
    {
        //Проверям достигла ли наша сфера следующую точку _newCoordinatesOfClick[1]
        if (Vector2.Distance(transform.position, _newCoordinatesOfClick[1]) < 10)
        {
            //Удаляем нулевой элемент - чистим лист
            _newCoordinatesOfClick.RemoveAt(0);
            _ropeRenderer.positionCount = _newCoordinatesOfClick.Count;
            //Переприсваиваем все координаты для _ropeRenderer
            for (int i = 1; i < _ropeRenderer.positionCount; i++)
            {
                _ropeRenderer.SetPosition(i, _newCoordinatesOfClick[i]);
            }
        }
    }

    //Метод изменения скорости шара от положения слайдера
    public void ChangeSphereSpeedMove()
    {
        _speedOfSphere = _sliderMoveSpeed.value;
    }
}


