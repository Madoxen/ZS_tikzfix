namespace TikzFix.Model.Tool
{
    enum ShapeState
    {
        //Initial phase of drawing (usually means that user clicked once)
        START, 

        // shape is being drawn
        DRAWING,

        // drawing shape is finished, after next action 
        // tool will start drawing new shape
        // this will commit the shape to the canvas shape registry
        FINISHED
    }
}
