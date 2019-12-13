import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Post } from '../models/post';
import { Tag } from '../models/tag';
import { map, flatMap, catchError } from 'rxjs/operators';
import { Observable, of, Subject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PostService {
  public post: Post;
  public score: number = 0;
  public tags: Tag[];
  public answers: Post[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {  }

  getAllPosts() {
    return this.http.get<Post[]>(`${this.baseUrl}api/posts`).pipe(
      map(res => res.map(p => new Post(p)))
    );
  }

  // getPost(post: Post) {
  //   return this.http.get<Post>(`${this.baseUrl}api/posts/${post.id}`).pipe(
  //     map(p => !p ? null : new Post(p)),
  //     catchError(err => of(null))
  //   );
  // }

  setScore() {
    console.log("VOTE: avant", this.post.votes.length);
    this.post.votes.forEach(v => {
      console.log("VOTE: " + v);
      this.score += v.upDown;
    });
    console.log("SCORE: " + this.score);
  }

  setPostDetail(p: Post) {
    this.post = p;
    this.setScore();
  }

  getAllTags() {
    return this.http.get<Tag[]>(`${this.baseUrl}api/posts`).pipe(
      map(res => res.map(p => new Post(p).tags))
    );
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
}