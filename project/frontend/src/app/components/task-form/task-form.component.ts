import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskService } from '../../services/task.service';
import { Task } from '../../models/task.model';

@Component({
  selector: 'app-task-form',
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.scss']
})
export class TaskFormComponent implements OnInit {
  @Input()  task: Task | null = null;
  @Output() saved    = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();

  form!: FormGroup;
  submitting = false;
  errorMsg: string | null = null;

  get isEdit(): boolean { return !!this.task; }

  constructor(private fb: FormBuilder, private taskService: TaskService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      title:       [this.task?.title       ?? '', [Validators.required, Validators.maxLength(255)]],
      description: [this.task?.description ?? ''],
      priority:    [this.task?.priority    ?? 'normal', Validators.required],
      status:      [this.task?.status      ?? 'nao_iniciado', Validators.required],
    });
  }

  submit(): void {
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }

    this.submitting = true;
    this.errorMsg   = null;
    const dto = this.form.value;

    const request = this.isEdit
      ? this.taskService.update(this.task!.id, dto)
      : this.taskService.create(dto);

    request.subscribe({
      next:  () => { this.submitting = false; this.saved.emit(); },
      error: () => { this.submitting = false; this.errorMsg = 'Erro ao salvar tarefa.'; }
    });
  }

  hasError(field: string): boolean {
    const ctrl = this.form.get(field);
    return !!(ctrl?.invalid && ctrl?.touched);
  }
}
