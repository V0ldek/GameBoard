using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoard.LogicLayer.GameEventLogic.Dtos
{
    public enum GameEventPermission
    {
        Creator,
        AcceptedInvitation,
        PendingInvitation,
        Forbidden
    }
}
