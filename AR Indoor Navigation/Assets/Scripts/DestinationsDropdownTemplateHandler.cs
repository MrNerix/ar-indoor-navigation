using UnityEngine;

public class DestinationsDropdownTemplateHandler : MonoBehaviour
{
    public GameObject favIcon;
    
    private void OnDisable()
    {
        Debug.Log("Loading Favourites from file on hiding the destinations dropdown menu and ");
        FavIconHandler favIconHandler = favIcon.GetComponent<FavIconHandler>();
        favIconHandler.LoadFavouritesOptionsFromFile();
    }
}
