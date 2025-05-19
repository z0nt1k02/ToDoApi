﻿namespace ToDoApi.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;

    public List<NoteModel> Notes { get; set; } = [];
}