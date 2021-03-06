import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from '../components/home/home.component';
import { UserListComponent } from '../components/userList/userList.component';
import { TagListComponent } from '../components/tagList/tagList.component';
import { TagQuestionsComponent } from '../components/tagQuestions/tagQuestions.component';
import { LoginComponent } from '../components/login/login.component';
import { RestrictedComponent } from '../components/restricted/restricted.component';
import { SignUpComponent } from '../components/signup/signup.component';
import { CreatePostComponent } from '../components/create-post/create-post.component';
import { EditPostComponent } from '../components/edit-post/edit-post.component';
import { EditCommentComponent } from '../components/edit-comment/edit-comment.component';
import { PostDetailComponent } from '../components/postDetail/postDetail.component';
import { UnknownComponent } from '../components/unknown/unknown.component';
import { AuthGuard } from '../services/auth.guard';
import { Role } from '../models/user';
import { ProfileComponent } from '../components/profile/profile.component';

const appRoutes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'signup', component: SignUpComponent },
  { path: 'create-post', component: CreatePostComponent },
  { path: 'edit-comment', component: EditCommentComponent },
  { path: 'edit-post', component: EditPostComponent },
  { path: 'profile', component: ProfileComponent },
  {
    path: 'users',
    component: UserListComponent,
    canActivate: [AuthGuard],
    data: { roles: [Role.Admin] }
  },
  {
    path: 'tags',
    component: TagListComponent,
    //canActivate: [AuthGuard],
  },
  {
    path: 'tagQuestions',
    component: TagQuestionsComponent,
    //canActivate: [AuthGuard],
  },
  {
    path: 'postdetail',
    component: PostDetailComponent,
    //canActivate: [AuthGuard],
  },
  {
    path: 'login',
    component: LoginComponent
  },
  { path: 'restricted', component: RestrictedComponent },
  { path: '**', component: UnknownComponent }
];

export const AppRoutes = RouterModule.forRoot(appRoutes);