export type TaskPriority = 'baixa' | 'normal' | 'alta';
export type TaskStatus   = 'nao_iniciado' | 'em_progresso' | 'concluido';

export interface Task {
  id:          number;
  title:       string;
  description: string | null;
  priority:    TaskPriority;
  status:      TaskStatus;
  createdAt:   string;
  updatedAt:   string;
}

export interface CreateTaskDto {
  title:       string;
  description: string | null;
  priority:    TaskPriority;
  status:      TaskStatus;
}

export type UpdateTaskDto = CreateTaskDto;

export const PRIORITY_LABELS: Record<TaskPriority, string> = {
  baixa:  'Baixa',
  normal: 'Normal',
  alta:   'Alta',
};

export const STATUS_LABELS: Record<TaskStatus, string> = {
  nao_iniciado: 'Não Iniciado',
  em_progresso: 'Em Progresso',
  concluido:    'Concluído',
};
