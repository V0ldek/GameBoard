using System;
using System.Collections.Generic;
using System.Text;

namespace GameBoard.DataLayer.Entities
{
    //Shall I create a class GameEventTab and then derivative class GameEventDescriptionTab?
    public class DescriptionTab
    {
        //With One-to-One there is no need for any other key different than foreign key.
        //However One-to-One is pretty redundant, because it is sufficient to store a description in GameEvent.
        public int DescriptionOfId { get; set; }

        //This way of naming is to code dependent for me; 
        //Shall be changed to GameEvent?
        public GameEvent DescriptionOf { get; set; }

        public string Description { get; set; }
    }
}
