using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour
{

    // array will store prefabs
    [SerializeField]
    private GameObject[] availableRooms;
    // list will store instanced rooms to see if more rooms need to be added
    [SerializeField]
    private List<GameObject> currentRooms;
    // store scren width for checking positions
    private float screenWidth;

    // array for storing objects that can be generated - enemies, bucks and PUPs
    [SerializeField]
    private GameObject[] availableObjects;
    // list will store instanced objects
    [SerializeField]
    private List<GameObject> objects;

    // used to control distance range between objects
    [SerializeField]
    private float objectsMinDistance = 5.0f;
    [SerializeField]
    private float objectsMaxDistance = 10.0f;

    // used to set min or max height of the object
    [SerializeField]
    private float objectsMinY = -1.4f;
    [SerializeField]
    private float objectsMaxY = 1.4f;

    // Start is called before the first frame update
    void Start()
    {
        // calculate the size of the screen width
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidth = height * Camera.main.aspect;

        // whilst the game is runnning the room status will be checked periodically
        StartCoroutine(GeneratorCheck());
    }

    void AddObject(float lastObjectX)
    {
        // generate random index for selection from array
        int randomIdx = Random.Range(0, availableObjects.Length);
        // create an instance of the randomly selected object
        GameObject obj = (GameObject)Instantiate(availableObjects[randomIdx]);
        // get random values to spawn object on x axis and y axis
        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
        float objectY = Random.Range(objectsMinY, objectsMaxY);
        // set object position using random values
        obj.transform.position = new Vector3(objectPositionX, objectY, 0);

        // add the object to the world
        objects.Add(obj);
    }

    void GenerateObjectsAsNeeded()
    {
        // calculate pixels ahead and behind player
        float playerX = transform.position.x;
        float removeObjectsX = playerX - screenWidth;
        float addObjectX = playerX + screenWidth;
        float farthestObjectX = 0;

        //
        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach (var obj in objects)
        {
            // the position of the object
            float objX = obj.transform.position.x;
            // 
            farthestObjectX = Mathf.Max(farthestObjectX, objX);
            // add object to list of objects to be removed
            if (objX < removeObjectsX)
            {
                objectsToRemove.Add(obj);
            }
        }

        //
        foreach(var obj in objectsToRemove)
        {
            objects.Remove(obj);
            // Destroy(obj);
        }

        // if furthest object is less than add object constrain then add object
        if(farthestObjectX < addObjectX)
        {
            AddObject(farthestObjectX);
        }


    }

    void AddRoom(float farthestRoomEndX)
    {

        // get random prefab to add
        int randomRoomIdx = Random.Range(0, availableRooms.Length);
        // create a room object from available array and use random index 
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIdx]);
        // get size of room from floor object 
        float roomWidth = room.transform.Find("floor").localScale.x;
        // get the center of the room to be able to position new room in correct position
        float roomCenter = farthestRoomEndX + roomWidth * 0.5f;
        // position the room
        room.transform.position = new Vector3(roomCenter, 0, 0);
        // add the new room to the list
        currentRooms.Add(room);

    }

    void GenerateRoomAsNeeded()
    {
        // create list for rooms to be removed
        List<GameObject> roomsToRemove = new List<GameObject>();
        // flag variable for creating rooms, set to false in foreach loop
        bool addRooms = true;
        // get player position on x axis
        float playerX = transform.position.x;
        // the position at which the room should be removed
        float removeRoomX = playerX - screenWidth;
        // the point at which new rooms should be added if passed
        float addRoomX = playerX + screenWidth;
        // the point at which the current room ends
        float farthestRoomEndX = 0;

        foreach (var room in currentRooms)
        {
            // use floor to get roomWidth and calculate where room starts and ends for room creation
            float roomWidth = room.transform.Find("floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;

            // if there is a room available then switch flag to false as a room is not needed
            if(roomStartX > addRoomX)
            {
                addRooms = false;
            }

            // if the room is past the removeRoomX point then it can be added to list for removal
            if(roomEndX < removeRoomX)
            {
                roomsToRemove.Add(room);
            }

            // furthest right point of the level is used to add room to the end of the level on creation
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);

        }

        // this removes any rooms in the rooms to be removed
        foreach(var room in roomsToRemove)
        {
            currentRooms.Remove(room);
            // if issues with APK build then Destroy(room) line can be commented out
            // Destroy(room);
        }

        // this will be true if a room is not detected near level end so a new room will be added at end of level
        if(addRooms)
        {
            AddRoom(farthestRoomEndX);
        }
    }

    private IEnumerator GeneratorCheck()
    {
        while(true)
        {
            GenerateRoomAsNeeded();
            GenerateObjectsAsNeeded();
            // add pause before executing room check again
            yield return new WaitForSeconds(0.25f);
        }
    }
}
