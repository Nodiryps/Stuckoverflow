import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, flatMap, catchError } from 'rxjs/operators';
import { Observable, of, Subject } from 'rxjs';
import { AuthenticationService } from './authentication.service';

import { Post } from '../models/post';
import { Tag } from '../models/tag';
import { Comment } from '../models/comment';
import { Vote } from '../models/vote';
import { MatTableDataSource } from '@angular/material';

@Injectable({ providedIn: 'root' })
export class PostService {
  public post: Post;
  public score: number = 0;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private authService: AuthenticationService) { }

  getPostsByUserId(userId: number) {
    return this.http.get<Post[]>(`${this.baseUrl}api/posts/usersPosts/${userId}`).pipe(
      map(res => res.map(p => new Post(p)))
    );
  }

  getById(id: number) {
    return this.http.get<Post>(`${this.baseUrl}api/posts/${id}`).pipe(
      map(u => !u ? null : new Post(u)),
      catchError(err => of(null))
    );
  }

  getAllQuestions() {
    return this.http.get<Post[]>(`${this.baseUrl}api/posts`).pipe(
      map(res => res.map(p => new Post(p)))
    );
  }

  getAllTags() {
    return this.http.get<Tag[]>(`${this.baseUrl}api/tags`).pipe(
      map(res => res.map(t => new Tag(t)))
    );
  }

  getAllAnswers() {
    return this.http.get<Post[]>(`${this.baseUrl}api/posts/answers/${this.post.id}`).pipe(
      map(res => res.map(p => new Post(p)))
    );
  }

  getAllQuestionsUnanswered(id: number) {
    return this.http.get<Post[]>(`${this.baseUrl}api/posts/answers/${id}`).pipe(
      map(res => res.map(p => new Post(p)))
    );
  }

  getAllComments(id: number) {
    return this.http.get<Comment[]>(`${this.baseUrl}api/comments/bypostid/${id}`).pipe(
      map(res => res.map(c => new Comment(c)))
    );
  }

  setScore() {
    this.score = 0;
    this.post.votes.forEach(v => {
      this.score += v.upDown;
    });
  }

  setPostDetail(p: Post) {
    this.post = p;
    this.setScore();
  }

  public add(p: Post): Observable<boolean> {
    return this.http.post<Post>(`${this.baseUrl}api/posts`, p).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

  public addComment(c: Comment): Observable<boolean> {
    return this.http.post<Comment>(`${this.baseUrl}api/comments`, c).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

  public update(p: Post): Observable<boolean> {
    return this.http.put<Post>(`${this.baseUrl}api/posts/${p.id}`, p).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

  public updateComment(c: Comment): Observable<boolean> {
    return this.http.put<Comment>(`${this.baseUrl}api/comments/${c.id}`, c).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

  public delete(p: Post): Observable<boolean> {
    return this.http.delete<boolean>(`${this.baseUrl}api/posts/${p.id}`).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }
  public deleteComment(c: Comment): Observable<boolean> {
    return this.http.delete<boolean>(`${this.baseUrl}api/comments/${c.id}`).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }
}