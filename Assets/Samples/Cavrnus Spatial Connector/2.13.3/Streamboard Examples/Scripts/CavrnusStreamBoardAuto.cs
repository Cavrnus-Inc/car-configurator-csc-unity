using System;
using System.Collections.Generic;
using CavrnusSdk.API;
using UnityEngine;

namespace CavrnusSdk.StreamBoards
{
    [RequireComponent(typeof(CavrnusStreamBoard))]
    public class CavrnusStreamBoardAuto : MonoBehaviour
    {
        private readonly Dictionary<CavrnusUser, IDisposable> streamHooks = new Dictionary<CavrnusUser, IDisposable>();

        private CavrnusStreamBoard board;
        private CavrnusSpaceConnection spaceConn;
        private IDisposable listDisp;

        private void Awake()
        {
            board = GetComponent<CavrnusStreamBoard>();
        }
        
        private void Start()
        {
			CavrnusFunctionLibrary.AwaitAnySpaceConnection(csc => {
                spaceConn = csc;
                listDisp = CavrnusFunctionLibrary.BindSpaceUsers(csc, UserAdded, UserRemoved);
            });
        }

        private void UserAdded(CavrnusUser user)
        {
            // This does not provide any board ownership. User B can and will override User A presentation.
            streamHooks.Add(user, user.BindUserVideoFrames(vidTex => board.UpdateTexture(vidTex)));
        }

        private void UserRemoved(CavrnusUser user)
        {
            if (streamHooks.TryGetValue(user, out var hook))
                hook?.Dispose();
        }
        
        private void OnDestroy()
        {
            listDisp?.Dispose();
        }
    }
}