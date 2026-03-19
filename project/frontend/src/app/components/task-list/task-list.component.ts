import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { Task, PRIORITY_LABELS, STATUS_LABELS, TaskPriority, TaskStatus } from '../../models/task.model';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss']
})
export class TaskListComponent implements OnInit {
  tasks: Task[]       = [];
  loading             = false;
  error: string | null = null;

  showForm            = false;
  editingTask: Task | null = null;

  priorityLabels = PRIORITY_LABELS;
  statusLabels   = STATUS_LABELS;

  filterStatus: TaskStatus | '' = '';
  filterPriority: TaskPriority | '' = '';

  constructor(private taskService: TaskService) {}

  ngOnInit(): void { this.loadTasks(); }

  loadTasks(): void {
    this.loading = true;
    this.taskService.getAll().subscribe({
      next: tasks => { this.tasks = tasks; this.loading = false; },
      error: ()    => { this.error = 'Erro ao carregar tarefas.'; this.loading = false; }
    });
  }

  get filteredTasks(): Task[] {
    return this.tasks.filter(t => {
      const okStatus   = !this.filterStatus   || t.status   === this.filterStatus;
      const okPriority = !this.filterPriority || t.priority === this.filterPriority;
      return okStatus && okPriority;
    });
  }

  openCreate(): void { this.editingTask = null; this.showForm = true; }

  openEdit(task: Task): void { this.editingTask = { ...task }; this.showForm = true; }

  onFormSave(): void { this.showForm = false; this.loadTasks(); }

  onFormCancel(): void { this.showForm = false; }

  deleteTask(id: number): void {
    if (!confirm('Remover esta tarefa?')) return;
    this.taskService.delete(id).subscribe({
      next: () => this.loadTasks(),
      error: () => alert('Erro ao remover tarefa.')
    });
  }

  priorityClass(p: TaskPriority): string {
    return { baixa: 'badge-low', normal: 'badge-normal', alta: 'badge-high' }[p];
  }

  statusClass(s: TaskStatus): string {
    return { nao_iniciado: 'status-pending', em_progresso: 'status-progress', concluido: 'status-done' }[s];
  }
}
