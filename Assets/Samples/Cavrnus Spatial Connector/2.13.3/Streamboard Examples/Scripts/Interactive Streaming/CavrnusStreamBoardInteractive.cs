using UnityEngine;
using CavrnusSdk.API;

namespace CavrnusSdk.StreamBoards
{
    [RequireComponent(typeof(CavrnusStreamBoard))]
    public class CavrnusStreamBoardInteractive : MonoBehaviour
    {
        [SerializeField] private CavrnusStreamContextMenu ctxMenuPrefab;

        private CavrnusStreamBoard board;
        private CavrnusSpaceConnection spaceConn;

        private void Awake()
        {
            board = GetComponent<CavrnusStreamBoard>();
        }
        
        private void Start()
        {
			CavrnusFunctionLibrary.AwaitAnySpaceConnection(csc => {
                spaceConn = csc;
            });
        }

        public void SelectStreamBoard()
        {
            if (spaceConn == null) 
                return;
            
            var ctx = Instantiate(ctxMenuPrefab, null);
            
            ctx.GetComponentInChildren<CavrnusStreamContextMenu>().Setup(spaceConn.GetCurrentSpaceUsers(), user => {
                board.UpdateAndBindUserTexture(user);

                Destroy(ctx.gameObject);
            });
        }
    }
}