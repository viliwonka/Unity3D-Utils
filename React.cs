using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class React {

    public struct ReAction {

        public Func<bool> react;
        public Action action;
    }

    List<ReAction> reActions;

    int pointer; //pointer to list
    ReAction currReaction;
    public React() {
        reActions = new List<ReAction>();
    }

    //First build reaction chain
    public void AddReaction(Func<bool> react, Action action) {

        ReAction newReAction = new ReAction();
        newReAction.react = react;
        newReAction.action = action;
        reActions.Add(newReAction);
        enabled = true;
    }

    bool enabled = false;

    void Next() {

        pointer = pointer + 1;

        if (pointer < reActions.Count) {
            currReaction = reActions[pointer];
        }
        else {
            enabled = false;
        }
    }

    public void Update() {
        if (enabled) {
            if (currReaction.react() || triggered) {

                //UnityEngine.Debug.Log("HI");

                currReaction.action();
                triggered = false;
                Next();
            }
        }
    }

    public void Enable() {
        pointer = 0;
        currReaction = reActions[pointer];
        enabled = true;
    }

    public void Disable() {
        enabled = false;
    }

    bool triggered = false;

    public void Trigger() {
        triggered = true;
    }
}