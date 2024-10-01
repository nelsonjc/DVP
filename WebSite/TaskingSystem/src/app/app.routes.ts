import { Routes } from '@angular/router';
import { authGuard } from './core/guard/auth.guard';
import { authenticatedGuard } from './core/guard/authenticated.guard';

export const routes: Routes = [
    {
        path: '',
        loadComponent: () => import('./shared/components/layout/layout.component'),
        children:[
            {
                path: 'dashboard',
                loadComponent: () => import('./business/dashboard/container/dashboard.component'),
                canActivate: [authGuard]
            },
            {
                path: 'tasks',
                loadComponent: () => import('./business/task/container/task.component'),
                canActivate: [authGuard]
            },
            {
                path: 'security',
                loadComponent: () => import('./business/security/container/security.component'),
                canActivate: [authGuard],
                data: { roles: ['Administrador'] }
            },
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full'
            },        
        ]
    },
    {
        path: 'login',
        loadComponent: () => import('./business/authentication/login/login.component'),
        canActivate: [authenticatedGuard]
    },
    {
        path: '**',
        redirectTo: 'dashboard',
    },
];
