using System.Collections.Generic;
using System.Windows.Input;

public class CommandManager
{
    private List<Icommand> commands;

    public CommandManager()
    {
        commands = new List<Icommand>()                                                                                                                                                                                                                            ;
    }

    public void AddCommand(Icommand command)
    {
        commands.Add(command);
        command.Do();
    }

    public void UndoCommand()
    {
        if (commands.Count == 0) return;
        Icommand command = commands[^1];
        commands.RemoveAt(commands.Count - 1);
        command.Undo();

    }
}