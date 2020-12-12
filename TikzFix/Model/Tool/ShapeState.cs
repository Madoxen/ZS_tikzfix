namespace TikzFix.Model.Tool
{
    enum ShapeState
    {
        // when user stared drawing but nothing can be printed
        // e.g. user picked line and clicked at one point
        // before clicking second point line cannot be drawn
        EMPTY, 

        // shape can be drawn
        DRAWING,

        // drawing shape is finished, after next action 
        // tool will start drawing new shape
        FINISHED
    }
}
