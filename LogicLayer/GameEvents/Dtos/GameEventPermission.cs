using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoard.LogicLayer.GameEvents.Dtos
{
    public enum GameEventPermission
    {
        Creator,
        AcceptedInvitation,
        PendingInvitation,
        Forbidden
    }
}
